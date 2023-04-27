import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  @ViewChild('myDropdown') editForm: NavbarComponent | undefined;
  model: any = {};
  element: any;

  constructor(public accountService: AccountService, private router: Router) {}

  ngOnInit(): void {
    setInterval(
      () => (this.element = document.getElementById('myDropdown')),
      100
    );
    
  }

  login(): void {
    this.accountService.login(this.model).subscribe({
      next: () => {
        setInterval(
          () => (this.element = document.getElementById('myDropdown')),
          100
        );
        const myObj = localStorage.getItem('user');
        if (myObj != null) {
          const lastActive = JSON.parse(myObj).lastActive;
          if (lastActive == null) {
            this.router.navigateByUrl('/change-password');
          }
        }
      },
    });
  }

  dropdown(): void {
    this.element.classList.toggle('show');
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
