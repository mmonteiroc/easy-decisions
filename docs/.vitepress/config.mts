import { defineConfig } from 'vitepress'

export default defineConfig({
  title: "EasyDecisions",
  description: "A fluent, strongly-typed decision engine for .NET",
  base: '/easy-decisions/',
  themeConfig: {
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Guide', link: '/guide/introduction' }
    ],
    sidebar: [
      {
        text: 'Introduction',
        items: [
          { text: 'What is EasyDecisions?', link: '/guide/introduction' },
          { text: 'Getting Started', link: '/guide/getting-started' },
          { text: 'Instantiation & Evaluation', link: '/guide/instantiation' },
          { text: 'Hit Policies', link: '/guide/hit-policies' },
          { text: 'FEEL-like Helpers', link: '/guide/feel-helpers' }
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/mmonteiroc/easy-decisions' }
    ],
    footer: {
      message: 'Made with ❤️ in Switzerland',
      copyright: 'Copyright © 2026-present mmonteiroc'
    }
  }
})
