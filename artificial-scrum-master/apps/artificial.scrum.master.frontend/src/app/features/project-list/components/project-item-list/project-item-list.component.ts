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
import { GetUserProjectsResponseElement } from '../../models/user-project';
import { ProjectListDataService } from '../../services/project-list-data.service';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-project-item-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, ProjectListItemComponent],
  templateUrl: './project-item-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectItemListComponent implements OnInit {
  private readonly projectListDataService = inject(ProjectListDataService);
  private readonly toastService = inject(ToastService);

  #projects = signal<GetUserProjectsResponseElement[]>([]);
  public readonly projects = this.#projects.asReadonly();

  public ngOnInit(): void {
    this.projectListDataService
      .getProjectsEvents()
      .pipe()
      .subscribe({
        next: (events) => {
          this.#projects.set(events);
        },
        error: () =>
          this.toastService.openError('Error fetching project events'),
      });
  }
}
