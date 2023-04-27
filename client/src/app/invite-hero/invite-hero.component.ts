import { HttpClient, HttpRequest } from '@angular/common/http';
import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { MembersService } from '../_services/members.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-invite-hero',
  templateUrl: './invite-hero.component.html',
  styleUrls: ['./invite-hero.component.css'],
})
export class InviteHeroComponent {
  model: any = {};
  constructor(
    private heroService: MembersService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  inviteHero() {
    this.heroService.addUser(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('');
        this.toastr.success('Usser was added successfully');
      },
    });
  }
}
