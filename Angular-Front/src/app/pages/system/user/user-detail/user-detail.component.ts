import { Component, OnInit } from '@angular/core';
import { User } from '../../../../@core/data/entities/user';
import { UserService } from '../../../../@core/data/services/user.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { LocalDataSource } from 'ng2-smart-table';
import { CompanyService } from '../../../../@core/data/services/company.service';
import { Company } from '../../../../@core/data/entities/company';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RoleSelectComponent } from './role-select.component';
import { Role } from '../../../../@core/data/entities/role';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent implements OnInit {

  // 用户信息
  user: User = new User();
  //角色数据源
  source: LocalDataSource = new LocalDataSource();
  settings = {
    noDataMessage: '无数据',
    mode: 'external',
    actions: {
      columnTitle: '操作',
      edit: false
    },
    add: {
      addButtonContent: '<i class="nb-edit"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>'
    },
    columns: {
      Name: {
        title: '角色名',
        type: 'string',
      },
    }
  };
  //用户公司
  userCompany: Company = new Company();
  //所有公司信息
  companies: Company[] = new Array();

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private companyService: CompanyService,
    private modalService: NgbModal,
    private logService: LogService
  ) { }

  ngOnInit() {
    let userID: string;
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        userID = params.get('id');
      }
    );
    //获取用户角色信息
    this.userService.getUserByID(userID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.user = res.Data as User;
          this.userCompany = this.user.Company;
          this.source.load(this.user.Roles);
        }
      },
      error => {
        console.log(error);
      }
    );
    //获取用户公司信息
    this.companyService.getCompanyList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.companies = res.Data as Company[];
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  saveUser(): void {
    this.userService.updateUser(this.user).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "用户管理";
          log.Action = "更新用户";
          log.Message = "用户" + this.user.Login + "更新信息成功";
          this.logService.addLog(log).subscribe();

          this.userService.getUserByID(this.user.ID).subscribe(
            next => {
              if (next.IsSuccess) {
                this.user = next.Data as User;
                alert(this.user.Login + "保存成功");
              }
            },
            error => {
              console.error(error);
            }
          );
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  updateRoles(): void {
    const modalRef = this.modalService.open(RoleSelectComponent, {
      size: 'sm',
      backdrop: 'static',
      container: 'nb-layout',
    });
    // 模态窗口返回数据给主页面
    modalRef.result.then((result) => {
      if (result) {
        const newRoles: Role[] = result as Role[];
        this.userService.updateUserRoles({ user: this.user, roles: newRoles }).subscribe(
          res => {
            if (res.IsSuccess) {
              //记录日志
              let log: Log = new Log();
              log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
              //从UTC时间转换成北京时间
              log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
              log.Module = "用户管理";
              log.Action = "更新用户角色";
              log.Message = "用户" + this.user.Login + "角色更新为";
              for (let j = 0, len = newRoles.length; j < len; j++) {
                log.Message += newRoles[j].Name + "+";
              }
              this.logService.addLog(log).subscribe();

              this.userService.getUserByID(this.user.ID).subscribe(
                next => {
                  this.user = next.Data as User;
                  this.source.load(this.user.Roles);
                },
                error => {
                  console.error(error);
                }
              );
            }
          },
          error => {
            console.error(error);
          }
        );
      }
    }).catch(
      error => {
      }
    );
  }

  userCompanyChange(event: any): void {
    this.companyService.getCompanyByID(event).subscribe(
      res => {
        if (res.IsSuccess) {
          this.userCompany = res.Data as Company;
          this.userService.updateUserCompany({ user: this.user, company: this.userCompany }).subscribe(
            res => {
              if (res.IsSuccess) {
                //记录日志
                let log: Log = new Log();
                log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
                //从UTC时间转换成北京时间
                log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
                log.Module = "用户管理";
                log.Action = "更新用户公司";
                log.Message = "更新用户" + this.user.Login + "公司为" + this.userCompany.Name;
                this.logService.addLog(log).subscribe();
                console.log(res.Data);
              }
              else {
                console.log(res);
              }
            },
            error => {
              console.log(error);
            }
          );
        }
        else {
          console.log(res);
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  deleteRole(event: any) {
    if (window.confirm('确定要删除角色' + event.data.Name + '吗?')) {
      const index = this.user.Roles.indexOf(event.data);
      this.user.Roles.splice(index, 1);
      this.userService.updateUserRoles({ user: this.user, roles: this.user.Roles }).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "用户管理";
            log.Action = "删除用户角色";
            log.Message = "删除用户" + this.user.Login + "角色" + event.data.Name;
            this.logService.addLog(log).subscribe();

            this.userService.getUserByID(this.user.ID).subscribe(
              next => {
                if (next.IsSuccess) {
                  this.user = next.Data as User;
                  this.source.load(this.user.Roles);
                }
              }
            );
          }
        },
        error => {
          console.error(error);
        }
      );
    }
  }
}
