import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Company } from '../../../../@core/data/entities/company';
import { Router } from '@angular/router';
import { CompanyService } from '../../../../@core/data/services/company.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CompanyAddComponent } from '../company-add/company-add.component';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';


@Component({
  selector: 'ntrip-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.scss']
})
export class CompanyListComponent implements OnInit {

  companyList: Company[] = new Array();
  settings = {
    mode: 'external',
    noDataMessage: '无数据',
    actions: {
      columnTitle: '操作',
    },
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
    },
    columns: {
      Name: {
        title: '公司名',
        type: 'string',
      },
      Corporation: {
        title: '法人',
        type: 'string',
      },
      Field: {
        title: '行业',
        type: 'string',
      },
      Contract: {
        title: '联系人',
        type: 'string',
      },
      Phone: {
        title: '联系电话',
        type: 'string',
      },
      Address: {
        title: '地址',
        type: 'string',
      },
    },
  };

  constructor(
    private companyService: CompanyService,
    private modalService: NgbModal,
    private router: Router,
    private logService: LogService
  ) { }

  ngOnInit() {
    this.getAllCompanies();
  }

  getAllCompanies(): void {
    this.companyService.getCompanyList().subscribe(
      res => {
        if (res.IsSuccess) {
          this.companyList = res.Data as Company[];
        }
      },
      error => {
        console.error(error);
      }
    )
  }

  createCompany(): void {
    const modalRef = this.modalService.open(CompanyAddComponent, {
      size: 'lg',
      //是否可以点击背景退出
      backdrop: 'static',
      container: 'nb-layout',
      //是否可以按esc退出
      keyboard: false,
    });
    modalRef.result.then((result) => {
      if (result) {
        this.companyService.getCompanyList().subscribe(
          res => {
            if (res.IsSuccess) {
              this.companyList = res.Data as Company[];
            }
          },
          error => {
            console.log(error);
          }
        );
      }
    });
  }

  deleteCompany(event: any): void {
    if (window.confirm('确定要删除' + event.data.Name + '吗?')) {
      // 逻辑删除数据库数据
      this.companyService.deleteCompany(event.data.ID).subscribe(
        res => {
          if (res.IsSuccess) {
            //记录日志
            let log: Log = new Log();
            log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
            //从UTC时间转换成北京时间
            log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
            log.Module = "公司管理";
            log.Action = "删除公司";
            log.Message = "公司" + event.data.Name + "删除成功";
            this.logService.addLog(log).subscribe();
            // 刷新列表
            this.companyService.getCompanyList().subscribe(
              next => {
                if (next.IsSuccess) {
                  this.companyList = next.Data as Company[];
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
  }

  editCompany(event: any): void {
    this.router.navigate(['/pages/proxy/company/companyDetail', event.data.ID]);
  }

}
