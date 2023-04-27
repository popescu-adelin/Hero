import {
  Component,
  ElementRef,
  Renderer2,
  HostListener,
  OnInit,
  ViewChild,
} from '@angular/core';

import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { Hero } from '../_models/hero';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  updatedHero: Hero | undefined;
  hero: Hero | undefined;
  user: User | null = null;

  constructor(
    private accountService: AccountService,
    private heroService: MembersService,
    private renderer: Renderer2,
    private elementRef: ElementRef,
    private toastr: ToastrService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => (this.user = user),
    });
  }

  ngOnInit(): void {
    this.loadHero();
  }

  loadHero() {
    if (this.user == null) return;
    this.heroService.getHero(this.user.email).subscribe({
      next: (hero) => (this.hero = hero),
    });
  }

  updateHero() {
    this.updatedHero = { ...this.hero, ...this.editForm?.value };
    this.heroService.updateHero(this.editForm?.value).subscribe({
      next: () => {
        this.hero = this.updatedHero;
        if (this.hero) this.hero.lastActive = new Date();
        this.toastr.success('Profile updated successfully');
      },
    });
  }

  openTab(evt: any, tabName: string) {
    // Hide all tabcontent elements
    const tabcontent =
      this.elementRef.nativeElement.querySelectorAll('.tabcontent');
    for (let i = 0; i < tabcontent.length; i++) {
      this.renderer.setStyle(tabcontent[i], 'display', 'none');
    }

    // Remove the 'active' class from all tablinks elements
    const tablinks =
      this.elementRef.nativeElement.querySelectorAll('.tablinks');
    for (let i = 0; i < tablinks.length; i++) {
      this.renderer.removeClass(tablinks[i], 'active');
    }

    // Show the current tabcontent element, and add the 'active' class to the button that opened the tab
    const currentTab = this.elementRef.nativeElement.querySelector(
      `#${tabName}`
    );
    this.renderer.setStyle(currentTab, 'display', 'flex');
    evt.currentTarget.classList.add('active');
  }
}
