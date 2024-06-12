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
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { finalize } from 'rxjs';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, ProjectListItemComponent, RouterLink, TranslateModule],
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

  #loggedOutOfTaiga = signal<boolean>(false);
  public loggedOutOfTaiga = this.#loggedOutOfTaiga.asReadonly();

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
          this.#loggedOutOfTaiga.set(true),
      });
  }
}
