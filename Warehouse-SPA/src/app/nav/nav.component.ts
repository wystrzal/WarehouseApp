import { Component, OnInit, EventEmitter, Output } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { AddModalComponent } from "../production/add-modal/add-modal.component";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.css"]
})
export class NavComponent implements OnInit {
  @Output() refresh = new EventEmitter<MouseEvent>();
  bsModalRef: BsModalRef;
  lineName: string;
  amountOfRepeat: number;

  constructor(
    private authService: AuthService,
    private modalService: BsModalService
  ) {}

  ngOnInit() {
    this.lineName = this.authService.decodedToken.unique_name;
  }

  refreshPlan(event: MouseEvent) {
    this.refresh.emit(event);
  }

  logout() {
    this.authService.logout();
  }

  newOrder() {
    const initialState = { line: this.lineName };
    this.bsModalRef = this.modalService.show(AddModalComponent, {
      initialState
    });
  }
}
