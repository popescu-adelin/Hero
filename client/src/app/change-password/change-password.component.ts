import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css'],
})
export class ChangePasswordComponent {
  model: any = {};

  constructor(
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  changePassword() {
    this.accountService.changePassword(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('');
        this.toastr.success('User updated');
      },
    });
  }
}
