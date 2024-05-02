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
import { TranslateService } from '@ngx-translate/core';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, ProjectListItemComponent],
  templateUrl: './projects-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectsListComponent implements OnInit {
  private readonly translateService = inject(TranslateService);
  private readonly projectListDataService = inject(ProjectsListDataService);
  private readonly toastService = inject(ToastService);

  #projects = signal<UserProject[]>([]);
  public readonly projects = this.#projects.asReadonly();

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  public ngOnInit(): void {
    this.#isLoading.set(true);

    this.projectListDataService
      .getProjects()
      .pipe(finalize(() => this.#isLoading.set(false)))
      .subscribe({
        next: (project) => {
          this.#projects.set(project);
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('Projects.Error')
          ),
      });
  }
}
