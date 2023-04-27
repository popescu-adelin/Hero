import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Hero } from '../_models/hero';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  public heroes$: Observable<Hero[]> | undefined;

  constructor(private heroesService: MembersService) {}

  ngOnInit(): void {
    this.heroes$ = this.heroesService.getHeroes();
  }
}
