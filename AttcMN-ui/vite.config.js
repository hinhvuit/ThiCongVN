import { defineConfig, loadEnv } from 'vite';
import path from 'path';
import { fileURLToPath, URL } from 'node:url'
import createVitePlugins from './vite/plugins'

export default defineConfig(({ mode, command }) => {
  const env = loadEnv(mode, process.cwd());
  const { VITE_APP_ENV } = env;
  const isBuild = command === 'build'

  return {
    base: VITE_APP_ENV === 'production' ? '/admin' : '/',
    plugins: createVitePlugins(env, isBuild),
    resolve: {
      alias: {
        '~': fileURLToPath(new URL('./', import.meta.url)),
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
      extensions: ['.mjs', '.js', '.ts', '.jsx', '.tsx', '.json', '.vue'],
    },
    server: {
      port: 8888,
      host: true,
      open: true,
      proxy: {
        '/dev-api': {
          target: 'http://localhost:58595/',
          changeOrigin: true,
          rewrite: (p) => p.replace(/^\/dev-api/, ''),
        },
        '/tc-api': {
          target: 'http://10.132.166.128:9001/',
          changeOrigin: true,
          rewrite: (p) => p.replace(/^\/tc-api/, ''),
        },
      },
    },
    css: {
      postcss: {
        plugins: [
          {
            postcssPlugin: 'internal:charset-removal',
            AtRule: {
              charset: (atRule) => {
                if (atRule.name === 'charset') {
                  atRule.remove();
                }
              },
            },
          },
        ],
      },
    },
  };
});