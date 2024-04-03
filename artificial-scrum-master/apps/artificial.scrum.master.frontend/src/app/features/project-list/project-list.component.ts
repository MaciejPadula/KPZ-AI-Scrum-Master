import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectItemListComponent } from './components/project-item-list/project-item-list.component';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, ProjectItemListComponent],
  templateUrl: './project-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectListComponent {}
