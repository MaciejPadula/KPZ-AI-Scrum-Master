import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintPreviewDataService } from '../../services/sprint-preview.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { SprintPreview } from '../../models/sprint-preview';
import { ActivatedRoute } from '@angular/router';
import { SprintPreviewItemComponent } from '../sprint-preview-item/sprint-preview-item.component';

@Component({
  selector: 'app-sprint-preview',
  standalone: true,
  imports: [CommonModule, SprintPreviewItemComponent],
  templateUrl: './sprint-preview.component.html',
})
export class SprintPreviewComponent implements OnInit {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly sprintPreviewDataService = inject(SprintPreviewDataService);
  private readonly toastService = inject(ToastService);

  #sprints = signal<SprintPreview[]>([]);
  public readonly sprints = this.#sprints.asReadonly();

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const projectId = params['projectId'];
      this.loadSprintPreviews(projectId);
    });
  }

  private loadSprintPreviews(projectId: number): void {
    this.sprintPreviewDataService.getSprintPreviews(projectId).subscribe({
      next: (sprints) => {
        this.#sprints.set(sprints);
      },
      error: () =>
        this.toastService.openError('Error fetching sprint previews'),
    });
  }
}
