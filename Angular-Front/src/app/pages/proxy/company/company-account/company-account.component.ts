import { Component, OnInit } from '@angular/core';
import { BoolRenderComponent } from '../../accountSYS/account-syslist/bool-render.component';
import { DateRenderComponent } from '../../accountSYS/account-syslist/date-render.component';
import { AccountService } from '../../../../@core/data/services/account.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountAddComponent } from '../../account/account-add/account-add.component';
import { Company } from '../../../../@core/data/entities/company';
import { CompanyService } from '../../../../@core/data/services/company.service';

@Component({
  selector: 'ntrip-company-account',
  templateUrl: './company-account.component.html',
  styleUrls: ['./company-account.component.scss']
})
export class CompanyAccountComponent implements OnInit {

  //公司信息
  company: Company = new Company();
  //公司所有账号信息
  accountList: Account[] = new Array<Account>();
  settings = {
    mode: 'external',
    noDataMessage: '',
    actions: {
      columnTitle: '操作',
      add: false,
      delete: false,
      edit: false
    },
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
      saveButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    columns: {
      Name: {
        title: '账户名',
        type: 'string',
      },
      Password: {
        title: '密码',
        type: 'string',
        filter: false,
      },

      Register: {
        title: '注册时间',
        type: 'custom',
        filter: false,
        renderComponent: DateRenderComponent
      },
      Expire: {
        title: '过期时间',
        type: 'custom',
        filter: false,
        renderComponent: DateRenderComponent
      },
      LastLogin: {
        title: '上次登录时间',
        type: 'html',
        filter: false,
      },
      PasswordOvertime: {
        title: '密码输错时间',
        type: 'html',
        filter: false,
      },
      PasswordOvercount: {
        title: '密码输错次数',
        type: 'string',
        filter: false,
      },
      IsLocked: {
        title: '是否被锁定',
        filter: {
          type: 'checkbox',
          config: {
            true: 'true',
            false: 'false',
            resetText: '清除',
          },
        },
        editable: false,
        type: 'custom',
        renderComponent: BoolRenderComponent
      },
      IsOnline: {
        title: '是否在线',
        filter: {
          type: 'checkbox',
          config: {
            true: 'true',
            false: 'false',
            resetText: '清除',
          },
        },
        editable: false,
        type: 'custom',
        renderComponent: BoolRenderComponent
      },
    },
  };

  constructor(
    private accountService: AccountService,
    private companyService: CompanyService,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NgbModal,
  ) { }

  ngOnInit() {
    let companyID: string;
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        companyID = params.get('id');
      }
    );

    this.companyService.getCompanyByID(companyID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.company = res.Data as Company;
        }
      },
      error => {
        console.log(error);
      }
    );

    this.accountService.getAccountByCompanyID(companyID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.accountList = res.Data as Account[];
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  createAccount(): void {
    const modalRef = this.modalService.open(AccountAddComponent, {
      size: 'lg',
      backdrop: 'static',
      container: 'nb-layout',
    });
    modalRef.result.then((result) => {
      if (result) {
        this.accountService.getAccountList().subscribe(
          res => {
            if (res.IsSuccess) {
              this.accountList = res.Data as Account[];
            }
          },
          error => {
            console.log(error);
          }
        );
      }
    });
  }

  editAccount(event: any): void {
    this.router.navigate(['/pages/proxy/account/accountDetail', event.data.ID]);
  }

}
