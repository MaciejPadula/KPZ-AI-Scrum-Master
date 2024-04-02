import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-link',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './link.component.html',
})
export class LinkComponent {
  @Input()
  public href: string;
}
