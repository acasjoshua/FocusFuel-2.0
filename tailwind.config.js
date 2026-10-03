/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./Components/**/*.{razor,html,cs}'],
  theme: {
    extend: {
      fontFamily: { sans: ['Inter', 'system-ui', '-apple-system', 'Segoe UI', 'Roboto', 'sans-serif'] },
      colors: {
        ink: '#0a0a14', primary: '#030213', muted: '#717182',
        field: '#f3f3f5', line: '#e5e7eb', link: '#2563eb', tint: '#eef3ff',
      },
    },
  },
  plugins: [],
};
