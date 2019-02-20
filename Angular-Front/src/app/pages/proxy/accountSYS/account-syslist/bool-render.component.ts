import { Component, Input, OnInit } from '@angular/core';

interface ViewCell {
    value: boolean;
    rowData: any;
}

@Component({
    template: `
    {{renderValue}}
  `,
})
export class BoolRenderComponent implements ViewCell, OnInit {

    renderValue: string;

    @Input() value: boolean;
    @Input() rowData: any;

    ngOnInit() {
        if (this.value === true) {
            this.renderValue = '是';
        } else if (this.value === false) {
            this.renderValue = '否';
        } else {
            this.renderValue = '未知';
        }
    }
}