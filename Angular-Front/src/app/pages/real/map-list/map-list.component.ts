import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { GGAHistoryService } from '../../../@core/data/services/gga-history.service';
import { Account } from '../../../@core/data/entities/account';
import { GGAHistory } from '../../../@core/data/entities/gga-history';
import { SessionHistory } from '../../../@core/data/entities/session-history';
import { environment } from '../../../../environments/environment';
import { SessionHistoryService } from '../../../@core/data/services/session-history.service';
import { User } from '../../../@core/data/entities/user';
import { Router } from '@angular/router';

@Component({
  selector: 'ntrip-map-list',
  templateUrl: './map-list.component.html',
  styleUrls: ['./map-list.component.scss'],
})
export class MapListComponent implements OnInit, AfterViewInit, OnDestroy {
  //地图对象
  map: any;
  //地图标记物数组对象
  markers: Array<any> = new Array<any>();
  //在线账户列表
  accountOnlineList: Array<Account> = new Array<Account>();
  //定时器
  private timer;
  //刷新时间
  public refreshTime: Date;
  //所有在线会话
  public sessionOnlineList: Array<SessionHistory> = new Array<SessionHistory>();
  //总体固定率
  public fixedRate: string;
  //所有在线会话的最新GGA信息
  public ggaList: Array<GGAHistory> = new Array<GGAHistory>();
  //登录用户信息
  user: User = new User();

  constructor(
    private ggaHistoryService: GGAHistoryService,
    private sessionHistoryService: SessionHistoryService,
    private router: Router,
  ) { }

  ngOnInit() {
    //初始化高德地图
    this.map = new AMap.Map('container', {
      resizeEnable: true,
      zoom: 9,
      //海南为中心
      center: [109.785258,19.205263]
    });
    this.user = JSON.parse(sessionStorage.getItem('loginUser')) as User;
    this.getFreshData();
  }

  ngAfterViewInit(): void {
    this.timer = setInterval(() => {//设置定时刷新事件，根据环境变量设置
      this.getFreshData();
    }, environment.refreshRate);
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  //获取在线账号和在账号最新概略位置
  getFreshData(): void {
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
    this.map.remove(this.markers);
    this.markers.length = 0;
    this.sessionHistoryService.getOnlineSessionByUserID(this.user.ID).subscribe(
      res => {
        if (res.IsSuccess) {
          this.sessionOnlineList = res.Data as SessionHistory[];
          this.sessionOnlineList.forEach(
            session => {
              this.ggaHistoryService.getGGAHistoryByAccount(session.AccountName).subscribe(
                next => {
                  if (next.IsSuccess) {
                    let ggaList = next.Data as GGAHistory[];
                    if (ggaList.length > 0) {
                      //显示在线账户位置信息到地图上
                      let marker = new AMap.Marker({
                        position: new AMap.LngLat(ggaList[0].Lng, ggaList[0].Lat),
                        title: ggaList[0].Account,
                        map: this.map,
                      });
                      marker.setLabel({
                        //修改label相对于maker的位置
                        offset: new AMap.Pixel(5, -20),
                        content: ggaList[0].Account,
                      });
                      //AMap.event.addListener(marker,'markerClick',this.trace(ggaList[0].Account));
                      this.markers.push(marker);
                    }
                  }
                },
                error => {
                  console.error(error);
                }
              );
            }
          );
        }
      },
      error => {
        console.error(error);
      });
  }

  trace(account: string): void {
    //this.router.navigate(['pages/real/accountTrace/traceDetail', account]);
    console.log(account);
  }
}
