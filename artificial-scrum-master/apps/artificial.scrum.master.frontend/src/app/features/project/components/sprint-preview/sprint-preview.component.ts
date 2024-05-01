import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintPreview } from '../../models/sprint-preview';
import { SprintPreviewItemComponent } from '../sprint-preview-item/sprint-preview-item.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-sprint-preview',
  standalone: true,
  imports: [
    CommonModule,
    SprintPreviewItemComponent,
    MaterialModule,
    TranslateModule,
  ],
  templateUrl: './sprint-preview.component.html',
})
export class SprintPreviewComponent {
  public sprints = input.required<SprintPreview[]>();
}
