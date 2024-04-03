import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { ProjectListItemComponent } from '../project-list-item/project-list-item.component';
import { UserProject } from '../../models/user-project';
import { ProjectsListDataService } from '../../services/projects-list-data.service';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, ProjectListItemComponent],
  templateUrl: './projects-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectsListComponent implements OnInit {
  private readonly projectListDataService = inject(ProjectsListDataService);
  private readonly toastService = inject(ToastService);

  #projects = signal<UserProject[]>([]);
  public readonly projects = this.#projects.asReadonly();

  public ngOnInit(): void {
    this.projectListDataService.getProjects().subscribe({
      next: (project) => {
        this.#projects.set(project);
      },
      error: () => this.toastService.openError('Error fetching projects'),
    });
  }
}
