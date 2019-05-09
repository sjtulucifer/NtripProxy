import { Component, OnInit } from '@angular/core';
import { HighLevelService } from '../../../@core/data/services/high-level.service';
import { HighLevel } from '../../../@core/data/entity/high-level';

@Component({
  selector: 'ngx-change-high-level',
  templateUrl: './change-high-level.component.html',
  styleUrls: ['./change-high-level.component.scss'],
  providers: [HighLevelService],
})
export class ChangeHighLevelComponent implements OnInit {

  uploadFileName: string;
  //转换前的高程点对象
  beforeHighLevel: HighLevel;
  //转换后的高程点对象
  afterHighLevel: HighLevel;

  constructor(
    private service: HighLevelService
  ) {
  }

  ngOnInit() {
    this.beforeHighLevel = new HighLevel();
    this.afterHighLevel = new HighLevel();
  }

  computeHighLevel(): void {
    this.service.ComputeHighLevel(this.beforeHighLevel).subscribe(
      res => {
        this.afterHighLevel = res as HighLevel;
      },
      error => {
        console.log(error);
      }
    )
  }

  uploadFile(event: any): void {
    var files = event.srcElement.files;
    let file: File = files[0];
    //用时间毫秒数来定义上传文件名
    this.uploadFileName = Date.now().toString();
    //console.log(this.uploadFileName);
    let formData: FormData = new FormData();
    formData.append('uploadFile', file, this.uploadFileName);
    this.service.UploadHighLevelFile(this.uploadFileName, formData).subscribe(
      res => {
        let errorLineNo = res as number;
        if (errorLineNo === 0) {
          alert("文件上传成功");
        } else {
          //如果格式有错误则重置上传文件为空
          event.srcElement.value = ''
          alert("第" + errorLineNo + "行格式错误");
        }
      },
      error => {
        console.log(error);
      }
    );
  }

  computeUploadFile(event: any): void {
    if (this.uploadFileName) {
      this.service.ComputeHighLevelFile(this.uploadFileName).subscribe(
        res => {
          //将返回的数据转换成txt文件下载
          let file = new File([res], "计算结果.txt", { type: "text/csv" });
          var a = document.createElement('a');
          a.href = URL.createObjectURL(file);
          a.download = "计算结果.txt";
          a.click();
        },
        error => {
          console.log(error);
        }
      );
    } else {
      alert("请上传文件");
    }

  }
}
