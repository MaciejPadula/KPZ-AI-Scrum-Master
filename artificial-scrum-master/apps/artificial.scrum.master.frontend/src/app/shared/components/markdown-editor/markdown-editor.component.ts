import {
  Component,
  computed,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-markdown-editor',
  standalone: true,
  imports: [CommonModule, FormsModule, MonacoEditorModule],
  templateUrl: './markdown-editor.component.html',
})
export class MarkdownEditorComponent {
  editorOptions = computed(() => {
    return {
      theme: document.body.classList.contains('dark-theme')
        ? 'vs-dark'
        : 'vs-light',
      language: 'markdown',
      lineNumbers: 'off',
      wordWrap: 'on',
      wrappingIndent: 'indent',
    };
  });

  @Input() editorContent: string;
  @Output() contentChange = new EventEmitter<string>();

  onContentChange(newValue: string) {
    this.contentChange.emit(newValue);
  }
}
