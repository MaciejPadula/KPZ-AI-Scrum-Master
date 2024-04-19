import {
  ChangeDetectionStrategy,
  Component,
  inject,
  Input,
} from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SprintPreview } from '../../models/sprint-preview';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { MaterialModule } from '../../../../shared/material.module';

@Component({
  selector: 'app-sprint-preview-item',
  standalone: true,
  imports: [CommonModule, AvatarComponent, MaterialModule],
  templateUrl: './sprint-preview-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SprintPreviewItemComponent {
  @Input()
  public sprintElement: SprintPreview;

  private readonly router = inject(Router);

  public formatDate(dateString: string): string {
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  }

  public redirectToUserStories(projectId: number, sprintId: number) {
    this.router.navigate([`/UserStories/${sprintId}`], {
      queryParams: { project: projectId },
    });
  }

  public get scrumServiceUrl() {
    const base = 'https://tree.taiga.io/project';
    return `${base}/${this.sprintElement.projectSlug}/taskboard/${this.sprintElement.sprintSlug}`;
  }
}
