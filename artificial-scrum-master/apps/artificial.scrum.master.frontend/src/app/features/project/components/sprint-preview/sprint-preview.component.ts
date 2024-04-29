import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintPreview } from '../../models/sprint-preview';
import { SprintPreviewItemComponent } from '../sprint-preview-item/sprint-preview-item.component';
import { MaterialModule } from '../../../../shared/material.module';

@Component({
  selector: 'app-sprint-preview',
  standalone: true,
  imports: [CommonModule, SprintPreviewItemComponent, MaterialModule],
  templateUrl: './sprint-preview.component.html',
})
export class SprintPreviewComponent {
  public sprints = input.required<SprintPreview[]>();
}
