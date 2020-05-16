import { Component, OnInit, ViewEncapsulation } from "@angular/core";

@Component({
  selector: "app-warehouse",
  templateUrl: "./warehouse.component.html",
  styleUrls: ["./warehouse.component.css"],
  encapsulation: ViewEncapsulation.None
})
export class WarehouseComponent implements OnInit {
  lineName = "bc4";

  ngOnInit() {}

  changeLine($event: { heading: string }) {
    this.lineName = $event.heading.toLowerCase();
  }
}
