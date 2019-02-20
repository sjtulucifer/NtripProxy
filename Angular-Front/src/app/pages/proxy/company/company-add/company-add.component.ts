import { Component, OnInit } from '@angular/core';
import { Company } from '../../../../@core/data/entities/company';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CompanyService } from '../../../../@core/data/services/company.service';
import { Logs } from 'selenium-webdriver';
import { LogService } from '../../../../@core/data/services/log.service';
import { Log } from '../../../../@core/data/entities/log';

@Component({
  selector: 'ntrip-company-add',
  templateUrl: './company-add.component.html',
  styleUrls: ['./company-add.component.scss']
})
export class CompanyAddComponent implements OnInit {

  // 公司信息
  company: Company = new Company();

  constructor(
    private activeModal: NgbActiveModal,
    private companyService: CompanyService,
    private logService: LogService
  ) { }

  ngOnInit() {
  }

  onClose(): void {
    this.activeModal.close();
  }

  onSubmit(): void {
    this.companyService.addCompany(this.company).subscribe(
      res => {
        if (res.IsSuccess) {
          //记录日志
          let log: Log = new Log();
          log.User = JSON.parse(sessionStorage.getItem('loginUser')).ID;
          //从UTC时间转换成北京时间
          log.Time = new Date(new Date().valueOf() + 8 * 3600 * 1000);
          log.Module = "公司管理";
          log.Action = "添加公司";
          log.Message = "公司" + this.company.Name + "添加成功";
          this.logService.addLog(log).subscribe();

          this.activeModal.close(this.company);
        }
      },
      error => {
        console.error(error);
      }
    );
  }
}
