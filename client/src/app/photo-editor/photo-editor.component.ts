import { Component, Input, OnInit } from '@angular/core';
import { Hero } from '../_models/hero';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';
import { take } from 'rxjs';
import { Photo } from '../_models/photo';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() hero: Hero | undefined;

  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user: User | undefined;

  constructor(
    private accountService: AccountService,
    private heroService: MembersService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) this.user = user;
      },
    });
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    this.heroService.setMainPhoto(photo.id).subscribe({
      next: () => {
        if (this.user && this.hero) {
          this.user.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
          this.hero.photoUrl = photo.url;
          this.hero.photos.forEach((p) => {
            if (p.isMain) p.isMain = false;
            if (p.id == photo.id) p.isMain = true;
          });
          this.hero.lastActive = new Date();
        }
      },
    });
  }

  deletePhoto(phitoId: number) {
    this.heroService.deletePhoto(phitoId).subscribe({
      next: () => {
        if (this.hero) {
          this.hero.photos = this.hero.photos.filter((x) => x.id != phitoId);
          this.hero.lastActive = new Date();
        }
      },
    });
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.hero?.photos.push(photo);
      }
    };
  }
}
