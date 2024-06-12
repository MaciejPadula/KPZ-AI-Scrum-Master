import {
  Component,
  computed,
  EventEmitter,
  inject,
  Input,
  Output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';
import { FormsModule } from '@angular/forms';
import { debounceTime, Subject } from 'rxjs';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-markdown-editor',
  standalone: true,
  imports: [CommonModule, FormsModule, MonacoEditorModule],
  templateUrl: './markdown-editor.component.html',
})
export class MarkdownEditorComponent {
  private readonly themeService = inject(ThemeService);

  private contentChangeSubject = new Subject<string>();

  constructor() {
    this.contentChangeSubject.pipe(debounceTime(500)).subscribe((newValue) => {
      this.contentChange.emit(newValue);
    });
  }

  editorOptions = computed(() => {
    return {
      theme: this.themeService.darkTheme() ? 'vs-dark' : 'vs-light',
      language: 'markdown',
      lineNumbers: 'off',
      wordWrap: 'on',
      wrappingIndent: 'indent',
    };
  });

  @Input() editorContent: string;
  @Output() contentChange = new EventEmitter<string>();

  onContentChange(newValue: string) {
    this.contentChangeSubject.next(newValue);
  }
}
