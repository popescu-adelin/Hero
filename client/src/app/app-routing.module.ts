import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { EditHeroComponent } from './edit-hero/edit-hero.component';
import { HomepageComponent } from './homepage/homepage.component';
import { InviteHeroComponent } from './invite-hero/invite-hero.component';
import { MemberDetailComponent } from './member-detail/member-detail.component';
import { MemberEditComponent } from './member-edit/member-edit.component';
import { MemberListComponent } from './member-list/member-list.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomepageComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'change-password', component: ChangePasswordComponent },
      { path: 'members', component: MemberListComponent },
      { path: 'members/:email', component: MemberDetailComponent },
      { path: 'invite', component: InviteHeroComponent },
      { path: 'edit', component: MemberEditComponent },
    ],
  },
  { path: '**', component: NotfoundComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
