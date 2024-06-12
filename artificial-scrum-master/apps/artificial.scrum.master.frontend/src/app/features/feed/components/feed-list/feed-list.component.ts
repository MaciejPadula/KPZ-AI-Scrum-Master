import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedDataService } from '../../services/feed-data.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { TimelineRow } from '../../../../shared/components/timeline-row/timeline-row';
import { finalize, map } from 'rxjs';
import { ProfileTimelineEvent } from '../../models/profile-timeline-event';
import { TimelineRowComponent } from '../../../../shared/components/timeline-row/timeline-row.component';
import { MaterialModule } from '../../../../shared/material.module';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-feed-list',
  standalone: true,
  templateUrl: './feed-list.component.html',
  imports: [CommonModule, TimelineRowComponent, MaterialModule, RouterLink, TranslateModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FeedListComponent implements OnInit {
  private readonly feedDataService = inject(FeedDataService);
  private readonly toastService = inject(ToastService);

  #feeds = signal<TimelineRow[]>([]);
  public readonly feeds = this.#feeds.asReadonly();

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  #loggedOutOfTaiga = signal<boolean>(false);
  public loggedOutOfTaiga = this.#loggedOutOfTaiga.asReadonly();

  public ngOnInit(): void {
    this.#isLoading.set(true);
    this.feedDataService
      .getFeedEvents()
      .pipe(
        map((events) => events.map((event) => this.mapToTimelineRow(event))),
        finalize(() => this.#isLoading.set(false))
      )
      .subscribe({
        next: (events) => {
          this.#feeds.set(events);
        },
        error: () => {
          this.#loggedOutOfTaiga.set(true);
        },
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
