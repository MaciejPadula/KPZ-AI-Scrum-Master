import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsListComponent } from './components/projects-list/projects-list.component';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, ProjectsListComponent],
  templateUrl: './projects.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectsComponent {}
