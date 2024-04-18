import { ChangeDetectionStrategy, Component, OnInit, inject, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Session } from '../../../../shared/models/session';
import { ProjectPokerDataService } from '../../services/project-poker-data.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { LinkComponent } from "../../../../shared/components/link/link.component";

@Component({
    selector: 'app-poker-sessions-list',
    standalone: true,
    templateUrl: './poker-sessions-list.component.html',
    imports: [CommonModule, MaterialModule, TranslateModule, LinkComponent],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PokerSessionsListComponent implements OnInit {
  private readonly pokerDataService = inject(ProjectPokerDataService);
  private readonly toastService = inject(ToastService);
  
  public projectId = input.required<number>();

  #sessions = signal<Session[]>([]);
  public readonly sessions = this.#sessions.asReadonly();

  public ngOnInit(): void {
    this.loadSessions();
  }

  public createSession() {
    this.pokerDataService.createSession("some name xddd", this.projectId())
      .subscribe({
        next: () => {
          this.toastService.openSuccess('Session created');
          this.loadSessions();
        },
        error: () => this.toastService.openError('Error creating session'),
      });
  }

  private loadSessions() {
    this.pokerDataService.getProjectSessions(this.projectId())
      .subscribe({
        next: (sessions) => {
          this.#sessions.set(sessions);
        },
        error: () => this.toastService.openError('Error fetching sessions'),
      });
  }
}
