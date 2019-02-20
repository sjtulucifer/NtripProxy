import { Component, Input, OnInit } from '@angular/core';

interface ViewCell {
    value: string;
    rowData: any;
}

@Component({
    template: `
    {{renderValue}}
  `,
})

export class DateRenderComponent implements ViewCell, OnInit {

    renderValue: string;

    @Input() value: string;
    @Input() rowData: any;

    ngOnInit() {
        this.renderValue = this.value.substring(0, this.value.indexOf('T'));
    }
}
