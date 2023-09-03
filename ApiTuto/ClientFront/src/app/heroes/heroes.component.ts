import { Component, OnInit } from '@angular/core';

import { Hero } from '../hero';
import { HeroService } from '../hero.service';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-heroes',
  templateUrl: './heroes.component.html',
  styleUrls: ['./heroes.component.css']
})
export class HeroesComponent implements OnInit {

  //selectedHero?: Hero;

  hheroes: Hero[] = [];

  constructor(private heroService: HeroService, private messageService: MessageService) { }// Permet de d'injecter le service 

  ngOnInit(): void {
    this.getHeroes();
  }

  // onSelect(hero: Hero): void {
  //   this.selectedHero = hero;
  //   this.messageService.add(`HeroesComponent: Selected hero id=${hero.id}`);
  // }

  add(name: string): void {
    name = name.trim();
    if (!name) { return; }
    this.heroService.addHero({ name } as Hero)
      .subscribe(hero => {
        this.hheroes.push(hero);
      });
  }

  delete(hero: Hero): void {
    this.hheroes = this.hheroes.filter(h => h !== hero);
    this.heroService.deleteHero(hero.id).subscribe();
  }

  getHeroes(): void {
    this.heroService.getHeroes()
        .subscribe(heroes => this.hheroes = heroes);
  }
} 

//Reserve the constructor for minimal initialization such as wiring constructor parameters to properties. The constructor shouldn't do anything. 


