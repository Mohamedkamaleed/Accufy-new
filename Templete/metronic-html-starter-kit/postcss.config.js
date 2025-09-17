import { createRequire } from 'module';
import { createRequire } from 'module';

var require = createRequire(import.meta.url);
var module = { exports: {} };

const require = createRequire(import.meta.url);

export const plugins = {
	'postcss-preset-env': {},
	'postcss-import': {},
	'tailwindcss/nesting': 'postcss-nesting',
	'postcss-preset-env': {
		features: { 'nesting-rules': false },
	},
	tailwindcss: {},
	autoprefixer: {},
};
