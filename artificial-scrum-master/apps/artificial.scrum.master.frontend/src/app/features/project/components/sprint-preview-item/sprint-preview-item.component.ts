import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintPreview } from '../../models/sprint-preview';

@Component({
  selector: 'app-sprint-preview-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sprint-preview-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SprintPreviewItemComponent implements OnInit {
  @Input()
  public sprintElement: SprintPreview;
  ngOnInit(): void {
    console.log('sprintElement');
    console.log(this.sprintElement);
  }

  public formatDate(dateString: string): string {
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day} ${month} ${year}`;
  }
}
