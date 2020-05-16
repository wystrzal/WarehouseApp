import { Component, OnInit } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import { PlanService } from "src/app/services/plan.service";

@Component({
  selector: "app-add-modal",
  templateUrl: "./error-modal.component.html",
  styleUrls: ["./error-modal.component.css"]
})
export class ErrorModalComponent implements OnInit {
  error: string;
  id: number;
  line: string;

  constructor(
    public bsModalRef: BsModalRef,
    private planService: PlanService
  ) {}

  confirmDelete(event: MouseEvent) {
    this.planService.deleteOrder(this.id, this.line).subscribe(
      () => {
        this.bsModalRef.hide();
        this.planService.refreshPlan.next(event);
      },
      error => {
        this.error = error;
        this.id = null;
      }
    );
  }

  ngOnInit() {}
}
