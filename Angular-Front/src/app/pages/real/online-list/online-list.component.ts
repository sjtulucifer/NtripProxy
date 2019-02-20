import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { environment } from '../../../../environments/environment.prod';
import { SessionHistoryService } from '../../../@core/data/services/session-history.service';
import { SessionHistory } from '../../../@core/data/entities/session-history';
import { AccountSYSService } from '../../../@core/data/services/account-sys.service';
import { Router } from '@angular/router';
import { User } from '../../../@core/data/entities/user';


@Component({
  selector: 'ntrip-online-list',
  templateUrl: './online-list.component.html',
  styleUrls: ['./online-list.component.scss'],
})
export class OnlineListComponent implements OnInit, AfterViewInit, OnDestroy {
  //定时器
  private timer;
  //刷新时间
  public refreshTime: Date;
  //总体固定率
  public fixedRate: string;
  //全部在线会话列表
  public onlineSessionList: Array<SessionHistory> = new Array<SessionHistory>();
  //固定解列表
  public fixedList: Array<SessionHistory> = new Array<SessionHistory>();
  //浮点解
  public floatList: Array<SessionHistory> = new Array<SessionHistory>();
  //差分解
  public differenceList: Array<SessionHistory> = new Array<SessionHistory>();
  //单点解
  public singleList: Array<SessionHistory> = new Array<SessionHistory>();
  //其他解
  public otherList: Array<SessionHistory> = new Array<SessionHistory>();
  //当前登录用户
  user: User = new User();

  settings = {
    mode: 'external',
    noDataMessage: '无数据',
    actions: {
      columnTitle: '操作',
      add: false,
      delete: false,
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
    },
    columns: {
      AccountName: {
        title: '账户名',
        type: 'string',
      },
      ConnectionStart: {
        title: '会话开始时间',
        type: 'html',
        filter: false
      },
      GGACount: {
        title: '发送GGA数量',
        type: 'string',
        filter: false
      },
      FixedCount: {
        title: '定位数量',
        type: 'string',
        filter: false
      },
    },
  };

  constructor(
    private router: Router,
    private sessionHistoryService: SessionHistoryService,
    private accountSYSService: AccountSYSService,
  ) { }

  ngOnInit() {
    this.user = JSON.parse(sessionStorage.getItem('loginUser')) as User;
    //获取总固定率
    this.sessionHistoryService.getOnlineSessionFixedRateByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.fixedRate = res.Data as string;
        }
      },
      error => {
        console.error(error);
      }
    );
    this.refreshTime = new Date();
    this.getGGAStatusList();
  }

  ngAfterViewInit(): void {
    this.timer = setInterval(() => {//设置定时刷新事件，根据环境变量设置
      //获取总固定率
      this.sessionHistoryService.getOnlineSessionFixedRateByUserID(this.user.ID).subscribe(
        res => {
          if (res.IsSuccess) {
            this.fixedRate = res.Data as string;
          }
        },
        error => {
          console.error(error);
        }
      );
      this.refreshTime = new Date();
      this.getGGAStatusList();
    }, environment.refreshRate);
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  getGGAStatusList(): void {
    //所有在线账号
    this.sessionHistoryService.getOnlineSessionByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.onlineSessionList = res.Data as SessionHistory[];
        }
      },
      error => {
        console.error(error);
      }
    );
    //单点解
    this.sessionHistoryService.getOnlineSessionSingleByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.singleList = res.Data as SessionHistory[];
        }
      },
      error => {
        console.error(error);
      }
    );
    //差分解
    this.sessionHistoryService.getOnlineSessionDifferenceByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.differenceList = res.Data as SessionHistory[];
        }
      },
      error => {
        console.error(error);
      }
    );
    //固定解
    this.sessionHistoryService.getOnlineSessionFixedByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.fixedList = res.Data as SessionHistory[];
        }
      },
      error => {
        console.error(error);
      }
    );
    //浮点解
    this.sessionHistoryService.getOnlineSessionFloatByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.floatList = res.Data as SessionHistory[];
        }
      },
      error => {
        console.error(error);
      }
    );
    //其他解
    this.sessionHistoryService.getOnlineSessionOtherByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.otherList = res.Data as SessionHistory[];
        }
      },
      error => {
        console.error(error);
      }
    );
  }

  traceAccount(event: any): void {
    this.router.navigate(['pages/real/accountTrace/traceDetail', event.data.AccountName]);
  }
}

