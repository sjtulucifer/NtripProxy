import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { CompanyService } from '../../../../@core/data/services/company.service';
import { Company } from '../../../../@core/data/entities/company';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-company-detail',
  templateUrl: './company-detail.component.html',
  styleUrls: ['./company-detail.component.scss']
})
export class CompanyDetailComponent implements OnInit {

  company: Company = new Company();
  parentCompany: Company = new Company();
  //所有公司信息
  otherCompanies: Company[] = new Array();

  constructor(
    private route: ActivatedRoute,
    private companyService: CompanyService,
    private logService: LogService
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
          this.parentCompany = this.company.ParentCompany;
        }
      },
      error => {
        console.error(error);
      }
    );

    this.companyService.getOtherCompanies(companyID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.otherCompanies = res.Data as Company[];
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  saveCompany(): void {
    this.companyService.updateCompany(this.company).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "公司管理";
          log.Action = "更新公司信息";
          log.Message = "公司" + this.company.Name + "信息更新成功";
          this.logService.addLog(log).subscribe();
          this.companyService.getCompanyByID(this.company.ID).subscribe(
            next => {
              if (next.IsSuccess) {
                this.company = next.Data as Company;
                alert(this.company.Name + "保存成功");
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

  parentCompanyChange(event: any): void {
    this.companyService.getCompanyByID(event).subscribe(
      res => {
        if (res.IsSuccess) {
          this.parentCompany = res.Data as Company;
          this.companyService.updateParentCompany({ company: this.company, parentCompany: this.parentCompany }).subscribe(
            res => {
              if (res.IsSuccess) {
                //记录日志
                let log: Log = new Log();
                log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
                //从UTC时间转换成北京时间
                log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
                log.Module = "公司管理";
                log.Action = "更新母公司信息";
                log.Message = "公司" + this.company.Name + "母公司更新为" + this.parentCompany.Name;
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
    )
  }
}
