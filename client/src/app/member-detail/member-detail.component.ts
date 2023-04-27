import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryAnimation, NgxGalleryImage } from '@kolkov/ngx-gallery';
import { Hero } from '../_models/hero';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  hero: Hero | undefined;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  constructor(
    private heroService: MembersService,
    private route: ActivatedRoute,
    private renderer: Renderer2,
    private elementRef: ElementRef
  ) {}

  ngOnInit(): void {
    this.loadMember();
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ];
  }

  getImages() {
    if (!this.hero) return [];
    const imageUrls = [];
    for (const photo of this.hero.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
      });
    }
    return imageUrls;
  }

  loadMember() {
    const email = this.route.snapshot.paramMap.get('email');
    if (!email) return;
    this.heroService.getHero(email).subscribe({
      next: (hero) => {
        this.hero = hero;
        this.galleryImages = this.getImages();
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
