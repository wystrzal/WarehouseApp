import { Component, OnInit, ViewChild, Input, ElementRef } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import { NgForm } from "@angular/forms";
import { PlanService } from "src/app/services/plan.service";

@Component({
  selector: "app-add-modal",
  templateUrl: "./add-modal.component.html",
  styleUrls: ["./add-modal.component.css"]
})
export class AddModalComponent implements OnInit {
  @ViewChild("addForm", { static: true }) editForm: NgForm;
  @ViewChild("focus", { static: false }) focus: ElementRef;
  model: any = {};
  repeatAmount: number;
  line: string;
  maxNum = 10;

  constructor(
    public bsModalRef: BsModalRef,
    private planService: PlanService
  ) {}

  ngOnInit() {
    setTimeout(() => {
      this.readCode();
    }, 1000);
  }

  readCode() {
    if (this.model.reference != this.focus.nativeElement.value) {
      "use_rfid()";
      this.model.reference = this.focus.nativeElement.value;
    }
    this.focus.nativeElement.select();
  }

  newOrder(event: MouseEvent) {
    this.planService
      .addNewOrder(this.model, this.repeatAmount, this.line)
      .subscribe();
    this.bsModalRef.hide();
    this.planService.refreshPlan.next(event);
  }
}
