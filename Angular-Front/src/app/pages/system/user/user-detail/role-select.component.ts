import { Component, OnInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { Role } from '../../../../@core/data/entities/role';
import { RoleService } from '../../../../@core/data/services/role.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  template:
    `
    <nb-card>
        <nb-card-body>
            <ng2-smart-table [settings]="settings" [source]="source" (userRowSelect)="selectRole($event)">
            </ng2-smart-table>
        </nb-card-body>
        <nb-card-footer>
            <button type="button" class="btn btn-primary" (click)="cancle()">取消</button>
            <button type="button" class="btn btn-primary" (click)="updateRole()">更新</button>
        </nb-card-footer>
    </nb-card>
    `,
})
export class RoleSelectComponent implements OnInit {

  source: LocalDataSource = new LocalDataSource();
  roleSelected: Role[] = new Array();

  settings = {
    selectMode: 'multi',
    noDataMessage: '无数据',
    actions: {
      add: false,
      edit: false,
      delete: false
    },
    columns: {
      Name: {
        title: '角色名',
        type: 'string',
      },
    }
  };

  constructor(
    private roleService: RoleService,
    private activeModal: NgbActiveModal,
  ) { }

  ngOnInit() {
    this.roleService.getRoleList().subscribe(
      res => {
        if (res.IsSuccess) {
          const roles = res.Data as Role[];
          this.source.load(roles);
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  selectRole(event: any): void {
    this.roleSelected = event.selected;
  }

  cancle(): void {
    this.activeModal.close();
  }

  updateRole(): void {
    this.activeModal.close(this.roleSelected);
  }
}
