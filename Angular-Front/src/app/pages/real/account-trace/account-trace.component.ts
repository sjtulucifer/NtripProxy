import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { GGAHistoryService } from '../../../@core/data/services/gga-history.service';
import { SessionHistoryService } from '../../../@core/data/services/session-history.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { GGAHistory } from '../../../@core/data/entities/gga-history';
import { SessionHistory } from '../../../@core/data/entities/session-history';

@Component({
  selector: 'ntrip-account-trace',
  templateUrl: './account-trace.component.html',
  styleUrls: ['./account-trace.component.scss']
})
export class AccountTraceComponent implements OnInit, AfterViewInit, OnDestroy {

  //定时器
  private timer: any;
  private accountName: string;
  //gga信息
  public gga: GGAHistory = new GGAHistory();
  //解状态显示信息
  public statusDisplay: string = "";
  //session信息
  public session: SessionHistory = new SessionHistory();
  //GGA信息数据列表,只保存最近的20个GGA数据
  public ggaList: Array<GGAHistory> = new Array<GGAHistory>();
  public ggaListDisplay: string = "";
  //历史会话状态
  public sessionHistoryList: Array<SessionHistory> = new Array<SessionHistory>();
  //固定率
  public fixedRate: number;

  settings2 = {
    noDataMessage: '无数据',
    actions: {
      columnTitle: '操作',
      add: false,
      delete: false,
      edit: false
    },
    columns: {
      AccountName: {
        title: '账户名',
        type: 'string',
        filter: false,
      },
      AccountType: {
        title: '账户验证',
        type: 'string',
        filter: false,
      },
      MountPoint: {
        title: '源节点',
        type: 'string',
        filter: false,
      },
      Client: {
        title: '终端',
        type: 'string',
        filter: false,
      },
      ClientAddress: {
        title: '地址',
        type: 'string',
        filter: false,
      },
      ConnectionStart: {
        title: '登录时间',
        type: 'string',
        filter: false,
        sortDirection: 'desc'
      },
      ConnectionEnd: {
        title: '注销时间',
        type: 'string',
        filter: false,
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
      ErrorInfo: {
        title: '错误信息',
        type: 'string',
        filter: false
      },
    },
  };

  constructor(
    private route: ActivatedRoute,
    private ggaHistoryService: GGAHistoryService,
    private sessionHistoryService: SessionHistoryService,
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(
      (params: ParamMap) => {
        this.accountName = params.get('accountName');
      }
    );
    if (this.accountName) {
      //获取第一次实时数据
      this.ggaHistoryService.getOnlineNewestGGAByAccount(this.accountName).subscribe(
        res => {
          if (res.IsSuccess) {
            this.gga = res.Data as GGAHistory;
            if (this.gga.Status == "1") {
              this.statusDisplay = "单点解";
            } else if (this.gga.Status == "2") {
              this.statusDisplay = "差分解";
            } else if (this.gga.Status == "5") {
              this.statusDisplay = "浮点解";
            } else if (this.gga.Status == "4") {
              this.statusDisplay = "固定解";
            } else {
              this.statusDisplay = "其他解";
            }
            if (this.gga.Session != null) {
                this.session = this.gga.Session;
                this.fixedRate = (this.session.FixedCount as number) / (this.session.GGACount as number);
              }
          }
        },
        error => {
          console.log(error);
        }
      );
      //获取历史数据
      this.sessionHistoryService.getSessionHistoryByAccount(this.accountName).subscribe(
        res => {
          if (res.IsSuccess) {
            this.sessionHistoryList = res.Data as SessionHistory[];
          }
        },
        error => {
          console.error(error);
        }
      );
    }
  }

  ngAfterViewInit(): void {
    this.timer = setInterval(() => {//设置定时刷新事件，根据环境变量设置
      if (this.accountName) {
        this.ggaHistoryService.getOnlineNewestGGAByAccount(this.accountName).subscribe(
          res => {
            if (res.IsSuccess) {
              this.gga = res.Data as GGAHistory;
              if (this.gga.Status == "1") {
                this.statusDisplay = "单点解";
              } else if (this.gga.Status == "2") {
                this.statusDisplay = "差分解";
              } else if (this.gga.Status == "5") {
                this.statusDisplay = "浮点解";
              } else if (this.gga.Status == "4") {
                this.statusDisplay = "固定解";
              } else {
                this.statusDisplay = "其他解";
              }
              if (this.gga.Session != null) {
                this.session = this.gga.Session;
                this.fixedRate = (this.session.FixedCount as number) / (this.session.GGACount as number);
                this.ggaList.push(this.gga);
                this.ggaListDisplay += this.gga.FixedTime + ':' + this.gga.GGAInfo + ';';
                if (this.ggaList.length >= 20) {
                  this.ggaList.length = 0;
                  this.ggaListDisplay = '';
                }
              }
            } else {
              this.gga = new GGAHistory();
              this.session = new SessionHistory();
              this.ggaList.length = 0;
              this.ggaListDisplay = '';
            }
          },
          error => {
            console.log(error);
          }
        );
      }
    }, environment.refreshRate);
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  trace(): void {
    this.accountName = this.gga.Account;
    if (this.accountName) {
      //获取第实时数据
      this.ggaHistoryService.getOnlineNewestGGAByAccount(this.accountName).subscribe(
        res => {
          if (res.IsSuccess) {
            this.gga = res.Data as GGAHistory;
            if (this.gga.Session != null) {
              this.session = this.gga.Session;
              this.fixedRate = (this.session.FixedCount as number) / (this.session.GGACount as number);
            }
          }
        },
        error => {
          console.log(error);
        }
      );
      //获取历史数据
      this.sessionHistoryService.getSessionHistoryByAccount(this.accountName).subscribe(
        res => {
          if (res.IsSuccess) {
            this.sessionHistoryList = res.Data as SessionHistory[];
          }
        },
        error => {
          console.error(error);
        }
      );
    }
  }
}
