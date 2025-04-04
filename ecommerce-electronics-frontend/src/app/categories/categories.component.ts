import { Component, OnInit } from '@angular/core';
import { CategoryService, Category } from '../category.service';
import { DataService } from '../data.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css'],
  template: `
    <h2>Categories</h2>
    <ul>
      <li *ngFor="let category of categories">
        {{ category.categoryName }}
      </li>
    </ul>
  `
})
export class CategoriesComponent implements OnInit {
  categories: Category[] = [];

  // constructor(private categoryService: CategoryService) { }
  constructor(private dataService: DataService) {}

  ngOnInit() {
    this.dataService.getCategories().subscribe(
      (data) => {
        this.categories = data;
      },
      (error) => {
        console.error('Error fetching categories:', error);
      }
    );
  }
}

