import { Component, OnInit, Input } from "@angular/core";
import { Permission } from "../../../../@core/data/entities/permission";
import { LocalDataSource } from "ng2-smart-table";
import { PermissionService } from "../../../../@core/data/services/permission.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
    template:
        `
    <nb-card>
        <nb-card-body>
            <ng2-smart-table [settings]="settings" [source]="source" (userRowSelect)="selectPermission($event)">
            </ng2-smart-table>
        </nb-card-body>
        <nb-card-footer>
            <button type="button" class="btn btn-primary" (click)="cancle()">取消</button>
            <button type="button" class="btn btn-primary" (click)="updatePermission()">添加</button>
        </nb-card-footer>
    </nb-card>
    `,
})
export class PermissionSelectComponent implements OnInit {

    source: LocalDataSource = new LocalDataSource();
    permissionSelected: Permission[] = new Array<Permission>();

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
                title: '权限名',
                type: 'string',
            },
        }
    };

    constructor(
        private permissionService: PermissionService,
        private activeModal: NgbActiveModal,
    ) { }

    ngOnInit(): void {
        this.permissionService.getPermissionList().subscribe(
            res => {
                if (res.IsSuccess) {
                    const updatePermissions = res.Data as Permission[];
                    this.source.load(updatePermissions);
                }
            },
            error => {
                console.log(error);
            }
        );
    }

    updatePermission(): void {
        this.activeModal.close(this.permissionSelected);
    }

    selectPermission(event: any): void {
        this.permissionSelected = event.selected;
    }

    cancle(): void {
        this.activeModal.close();
    }
}
