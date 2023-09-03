import { Component, Input } from '@angular/core';
import { Hero } from '../hero';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { HeroService } from '../hero.service';

@Component({
  selector: 'app-hero-detail',
  templateUrl: './hero-detail.component.html',
  styleUrls: ['./hero-detail.component.css']
})
export class HeroDetailComponent {


  @Input() heroe?: Hero;

  ngOnInit(): void {
    this.getHero();
  }
  
  getHero(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.heroService.getHero(id)
      .subscribe(hero => this.heroe = hero);
  }

  goBack(): void {
    this.location.back();
  }

  

  save(): void {
    if (this.heroe) {
      this.heroService.updateHero(this.heroe)
        .subscribe(() => this.goBack());
    }
  }

  

  constructor(
    private route: ActivatedRoute,
    private heroService: HeroService,
    private location: Location
  ) {}
  //The location is an Angular service for interacting with the browser. This service lets you navigate back to the previous view.
}
