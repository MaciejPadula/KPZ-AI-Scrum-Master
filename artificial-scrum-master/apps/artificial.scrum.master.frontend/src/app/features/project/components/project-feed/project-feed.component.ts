import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectDataService } from '../../services/project-data.service';
import { ActivatedRoute } from '@angular/router';
import { finalize, map } from 'rxjs';
import { ProfileTimelineEvent } from '../../models/profile-timeline-event';
import { TimelineRow } from '../../../../shared/components/timeline-row/timeline-row';
import { MaterialModule } from '../../../../shared/material.module';
import { TimelineRowComponent } from '../../../../shared/components/timeline-row/timeline-row.component';
import { ToastService } from '../../../../shared/services/toast.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-project-feed',
  standalone: true,
  imports: [
    CommonModule,
    MaterialModule,
    TimelineRowComponent,
    TranslateModule,
  ],
  templateUrl: './project-feed.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectFeedComponent implements OnInit {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly projectDataService = inject(ProjectDataService);
  private readonly toastService = inject(ToastService);

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  #feeds = signal<TimelineRow[]>([]);
  public readonly feeds = this.#feeds.asReadonly();

  public ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const projectId = params['projectId'];
      this.loadProject(projectId);
    });
  }

  private loadProject(projectId: number): void {
    this.#isLoading.set(true);
    this.projectDataService
      .getProjectEvents(projectId)
      .pipe(
        map((events) => events.map((event) => this.mapToTimelineRow(event))),
        finalize(() => this.#isLoading.set(false))
      )
      .subscribe({
        next: (events) => {
          this.#feeds.set(events);
        },
        error: () => this.toastService.openError('Error fetching feed events'),
      });
  }

  private mapToTimelineRow(event: ProfileTimelineEvent): TimelineRow {
    return {
      userName: event.userName,
      userNick: event.userNick,
      userAvatar: event.userPhoto,
      scrumObjectType: event.scrumObjectType,
      scrumObjectId: event.objectId,
      scrumObjectName: event.objectName,
      scrumObjectState: event.scrumObjectState,
      projectId: event.projectId,
      projectName: event.projectName,
    };
  }
}
