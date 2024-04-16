import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-story-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-story-list.component.html',
  styleUrl: './user-story-list.component.css',
})
export class UserStoryListComponent {}
