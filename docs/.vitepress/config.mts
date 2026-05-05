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
          { text: 'Getting Started', link: '/guide/getting-started' }
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/mmonteiroc/easy-decisions' }
    ]
  }
})
