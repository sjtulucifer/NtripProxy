import { Component, OnInit } from "@angular/core";
import { LocalDataSource } from "ng2-smart-table";
import { Menu } from "../../../../@core/data/entities/menu";
import { MenuService } from "../../../../@core/data/services/menu.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
    template:
        `
    <nb-card>
        <nb-card-body>
            <ng2-smart-table [settings]="settings" [source]="source" (userRowSelect)="selectMenu($event)">
            </ng2-smart-table>
        </nb-card-body>
        <nb-card-footer>
            <button type="button" class="btn btn-primary" (click)="cancle()">取消</button>
            <button type="button" class="btn btn-primary" (click)="updateMenu()">添加</button>
        </nb-card-footer>
    </nb-card>
    `,
})
export class MenuSelectComponent implements OnInit {

    source: LocalDataSource = new LocalDataSource();
    menuSelected: Menu[] = new Array<Menu>();

    settings = {
        selectMode: 'multi',
        noDataMessage: '无数据',
        actions: {
            add: false,
            edit: false,
            delete: false
        },
        columns: {
            Catagory: {
                title: '类别',
                type: 'string',
            },
            Name: {
                title: '菜单名',
                type: 'string',
            },           
        }
    };

    constructor(
        private menuService: MenuService,
        private activeModal: NgbActiveModal,
    ) { }

    ngOnInit(): void {
        this.menuService.getMenuList().subscribe(
            res => {
                if (res.IsSuccess) {
                    const updateMenus = res.Data as Menu[];
                    this.source.load(updateMenus);
                }
            },
            error => {
                console.log(error);
            }
        );
    }

    updateMenu(): void {
        this.activeModal.close(this.menuSelected);
    }

    selectMenu(event: any): void {
        this.menuSelected = event.selected;
    }

    cancle(): void {
        this.activeModal.close();
    }
}
