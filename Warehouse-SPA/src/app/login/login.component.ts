import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { ErrorService } from "../services/error.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent implements OnInit {
  model: any = {};

  constructor(
    private router: Router,
    private authService: AuthService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {}

  login() {
    this.authService.login(this.model).subscribe(
      next => {},
      error => {
        this.errorService.newError(error);
      },
      () => {
        const role = this.authService.decodedToken.role;
        if (role === "Production") {
          this.router.navigate(["production"]);
        } else if (role === "Warehouse") {
          this.router.navigate(["warehouse"]);
        }
      }
    );
  }
}
