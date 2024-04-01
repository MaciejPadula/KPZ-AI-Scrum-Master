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
import { map } from 'rxjs';
import { ProfileTimelineEvent } from '../../models/profile-timeline-event';
import { TimelineRowComponent } from '../../../../shared/components/timeline-row/timeline-row.component';

@Component({
  selector: 'app-feed-list',
  standalone: true,
  templateUrl: './feed-list.component.html',
  imports: [CommonModule, TimelineRowComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FeedListComponent implements OnInit {
  private readonly feedDataService = inject(FeedDataService);
  private readonly toastService = inject(ToastService);

  #feeds = signal<TimelineRow[]>([]);
  public readonly feeds = this.#feeds.asReadonly();

  public ngOnInit(): void {
    this.feedDataService
      .getFeedEvents()
      .pipe(
        map((events) => events.map((event) => this.mapToTimelineRow(event)))
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
      userAvatar: event.userAvatar,
      scrumObjectType: event.scrumObjectType,
      scrumObjectId: event.scrumObjectId,
      scrumObjectName: event.scrumObjectName,
      scrumObjectState: event.scrumObjectState,
      projectId: event.projectId,
      projectName: event.projectName,
    };
  }
}
