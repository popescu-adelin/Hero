import { Component, Input } from '@angular/core';
import { Hero } from '../_models/hero';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent {
  @Input() hero: Hero | undefined;
}
